import React, {useEffect, useState} from 'react';
import connection from '../Connection'

export function Status() {
    const [completedRooms, setCompletedRooms] = useState([]);
    const [timeoutPlayers, setTimeoutPlayers] = useState([]);

    useEffect(() => {
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
    })

    return (
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
    );
}