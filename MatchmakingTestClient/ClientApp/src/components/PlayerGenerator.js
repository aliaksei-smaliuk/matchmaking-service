import React, {useState} from 'react';

let timeout;
let count = 0;
let requestsPerMinuteGlobal;

function getRandomInt(max) {
    return Math.round(Math.random() * max);
}

const processInterval = async function (setPlayerCount) {
    count++;
    const request = {
        playerId: `playerId${count}`,
        level: getRandomInt(20),
        cash: getRandomInt(100),
        platform: getRandomInt(2),
        hoursInGame: getRandomInt(1000),
        gameType: getRandomInt(1)
    }
    const body = JSON.stringify(request)
    console.log(body)
    const init = {
        method: 'POST',
        body: body,
        headers: {
            'Content-Type': 'application/json'
        }
    }
    const response = await fetch('/matchmaking', init);
    setPlayerCount(count)
    timeout = setTimeout(() => processInterval(setPlayerCount), 60000 / requestsPerMinuteGlobal);
}

export function PlayerGenerator() {
    const [isGenerating, setIsGenerating] = useState(false);
    const [requestsPerMinute, setRequestsPerMinute] = useState(139);
    const [playerCount, setPlayerCount] = useState(0);

    function handleRequestsPerMinuteChanged(e) {
        setRequestsPerMinute(e.target.value);
    }

    function startProcessing() {
        requestsPerMinuteGlobal = requestsPerMinute;
        processInterval(setPlayerCount);
        setIsGenerating(true);
    }

    function stopProcessing() {
        clearTimeout(timeout);
        setIsGenerating(false);
    }

    return (
        <div className="row">
            <div className="col">
                {
                    !isGenerating
                        ? <button type="button" className="btn btn-success" onClick={startProcessing}>Start</button>
                        : <button type="button" className="btn btn-danger" onClick={stopProcessing}>Stop</button>
                }
            </div>
            <div className="col">
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <span className="input-group-text" id="basic-addon3">Matchmaking requests per minute</span>
                    </div>
                    <input type="text" className="form-control" id="basic-url" aria-describedby="basic-addon3"
                           disabled={isGenerating} value={requestsPerMinute} onChange={handleRequestsPerMinuteChanged}/>
                </div>
            </div>
            <div className="col">
                <div className="input-group mb-3">
                    <div className="input-group-prepend">
                        <span className="input-group-text" id="basic-addon3">Matchmaking requests generated</span>
                    </div>
                    <input type="text" className="form-control" id="basic-url" aria-describedby="basic-addon3" disabled
                           value={playerCount}/>
                </div>
            </div>
        </div>
    );
}