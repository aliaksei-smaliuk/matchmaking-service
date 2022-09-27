import React, {useEffect, useState} from 'react';
import * as signalR from "@microsoft/signalr";
import {PlayerGenerator} from "./PlayerGenerator";

function connectHub(hubUrl) {
    console.log(`Started ${hubUrl}`);
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl)
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Debug)
        .build();

    async function start() {
        try {
            console.log("SignalR Connecting.");
            await connection.start();
            console.log("SignalR Connected.");
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }

    connection.onclose(async () => {
        await start();
    });

    start();

    return connection;
}

export function Status() {
    const [connection, setConnection] = useState(null);
    const [completedRooms, setCompletedRooms] = useState([]);
    const [timeoutPlayers, setTimeoutPlayers] = useState([]);

    useEffect(async () => {
        const response = await fetch('/configuration');
        const configuration = await response.json();

        console.log(configuration)
        setConnection(connectHub(configuration.hubUrl))
    }, []);

    useEffect(() => {
        if (!connection)
            return;

        connection.on("RoomCompleted", data => {
            setCompletedRooms([data, ...completedRooms].slice(0, 30))
        });

        connection.on("TimeoutPlayer", data => {
            setTimeoutPlayers([data, ...timeoutPlayers].slice(0, 30))
        });

        return () => {
            connection.off("RoomCompleted");
            connection.off("TimeoutPlayer");
        }
    }, [connection])

    return (
        <div>
            <PlayerGenerator/>
            <div className="row">
                <div className="col">
                    <table className="table">
                        <thead>
                        <tr>
                            <th>Game rooms</th>
                        </tr>
                        </thead>
                        <tbody>
                        {completedRooms.map((message) => {
                            return <tr key={message}>
                                <td className="py-3">{message}</td>
                            </tr>
                        })}
                        </tbody>
                    </table>
                </div>
                <div className="col">
                    <table className="table">
                        <thead>
                        <tr>
                            <th>Timeout players</th>
                        </tr>
                        </thead>
                        <tbody>
                        {timeoutPlayers.map((message) => {
                            return <tr key={message}>
                                <td className="py-3">{message}</td>
                            </tr>
                        })}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
}