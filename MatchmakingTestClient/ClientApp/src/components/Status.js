import React, {useEffect, useState} from 'react';
import {HubConnectionBuilder} from "@microsoft/signalr";
import {PlayerGenerator} from "./PlayerGenerator";

export function Status() {
    const [connection, setConnection] = useState(null);
    const [completedRooms, setCompletedRooms] = useState([]);
    const [timeoutPlayers, setTimeoutPlayers] = useState([]);
    const [roomCompletedCount, setRoomCompletedCount] = useState(0);
    const [timeoutPlayersCount, setTimeoutPlayersCount] = useState(0);

    useEffect(() => {
        const initConnection = () => {
            return fetch('/configuration')
                .then(response => response.json())
                .then(configuration => {

                    const newConnection = new HubConnectionBuilder()
                        .withUrl(configuration.hubUrl)
                        .withAutomaticReconnect()
                        .build();

                    newConnection.start()
                    setConnection(newConnection);
                });
        }
        initConnection();
    }, []);

    useEffect(() => {
        if (!connection) {
            return;
        }

        connection.on("RoomCompleted", data => {
            setCompletedRooms([data, ...completedRooms])
            setRoomCompletedCount(roomCompletedCount + 1)
        });

        connection.on("TimeoutPlayer", data => {
            setTimeoutPlayers([data, ...timeoutPlayers])
            setTimeoutPlayersCount(timeoutPlayersCount + 1)
        })
        return () => {
            connection.off("RoomCompleted")
            connection.off("TimeoutPlayer")
        }
    }, [connection, completedRooms, roomCompletedCount, timeoutPlayers, timeoutPlayersCount]);

    return (
        <div>
            <PlayerGenerator/>
            <div className="row">
                <div className="col">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text" id="basic-addon3">Rooms completed count</span>
                        </div>
                        <input type="text" className="form-control" id="basic-url" aria-describedby="basic-addon3"
                               disabled
                               value={roomCompletedCount}/>
                    </div>
                </div>
                <div className="col">
                    <div className="input-group mb-3">
                        <div className="input-group-prepend">
                            <span className="input-group-text"
                                  id="basic-addon3">Timeout matchmaking requests count</span>
                        </div>
                        <input type="text" className="form-control" id="basic-url" aria-describedby="basic-addon3"
                               disabled
                               value={timeoutPlayersCount}/>
                    </div>
                </div>
            </div>
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