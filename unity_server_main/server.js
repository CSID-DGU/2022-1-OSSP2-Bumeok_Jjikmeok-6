const express = require('express')

const index = require('./router/index')

const app = express()

const crypto = require('crypto')

const bodyParser = require('body-parser')

const bodyParserErrorHandler = require('express-body-parser-error-handler')

const port = 3000

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