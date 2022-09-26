import React, {Component} from 'react';
import {Route} from 'react-router';
import {Layout} from './components/Layout';
import {Home} from './components/Home';
import {FetchData} from './components/FetchData';
import {Counter} from './components/Counter';

import './custom.css'
import * as signalR from "@microsoft/signalr";

const {env} = require('process');

let target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:22885';

target = "https://localhost:7114";
const hubUrl = `${target}/chathub`;
//const hubUrl = `/chathub`;
console.log(`Started ${hubUrl}`);
const connection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
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

connection.on("ReceiveMessage", data => {
    console.log(data);
});

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();


export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home}/>
                <Route path='/counter' component={Counter}/>
                <Route path='/fetch-data' component={FetchData}/>
            </Layout>
        );
    }
}
