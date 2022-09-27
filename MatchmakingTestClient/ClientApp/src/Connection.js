import * as signalR from "@microsoft/signalr";

const target = "";//"https://localhost:7114";
const hubUrl = `${target}/chathub`;
//const hubUrl = `/chathub`;
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

// Start the connection.
start();

export default connection;