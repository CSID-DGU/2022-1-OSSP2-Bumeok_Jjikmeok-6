const mysql = require('mysql2/promise')
const {host, user, password, database, port} = require('../DB_secret/secret_OSSW_DB')

const pool_k = mysql.createPool({
    host: host,
    user: user,
    password: password,
    database: database,
    port: port
})

module.exports = pool_k