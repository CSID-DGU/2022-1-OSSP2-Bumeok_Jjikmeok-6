const express = require('express')

const index = require('./router/index')

const app = express()

const bodyParser = require('body-parser')

const port = 3000

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({extended: true}))

app.use(index)

app.listen(port, () => {
    console.log(`success in port ${port}!`)
})