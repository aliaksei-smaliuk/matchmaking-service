'use strict';

const express = require('express');

// Constants
const PORT = 8080;
const HOST = '0.0.0.0';

// App
const app = express();
app.get('/', function(request, response){
    response.sendFile('/usr/src/app/index.html');
});

app.listen(PORT, HOST);
console.log(`Running on http://${HOST}:${PORT}`);