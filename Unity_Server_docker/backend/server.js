const express = require('express')

const index = require('./router/index')

const app = express()

const crypto = require('crypto')

const bodyParser = require('body-parser')

const bodyParserErrorHandler = require('express-body-parser-error-handler')

const port = process.env.NODE_LOCAL_PORT || 3000

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({extended: true}))
app.use(bodyParserErrorHandler())

app.use(index)

app.get('/', (req, res) => {
    crypto.pbkdf2('a', 'b', 100000, 512, 'sha512', (err, key) => {
        res.send('hello world!')
    })
})

app.listen(port, () => {
    console.log(`success in port ${port}!`)
})

var signals = {
    'SIGINT': 2,
    'SIGTERM': 15
};
shutdown = (signal, value) => {
    server.close(function () {
        console.log('server stopped by ' + signal);
        process.exit(128 + value);
    });
}
Object.keys(signals).forEach((signal) => {
    process.on(signal, function () {
        shutdown(signal, signals[signal]);
    });
});