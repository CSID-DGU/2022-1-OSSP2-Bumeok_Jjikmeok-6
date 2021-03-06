const passport = require('passport')

let LocalStrategy = require('passport-local').Strategy

const OSSW_DB_pool = require('../DB_info/open_OSSW_DB')

const crypto = require('crypto')

passport.use(new LocalStrategy(
    {
        usernameField: 'id',
        passwordField: 'pwd'
    },
    async (id, pwd, done) => {
    const connection = await OSSW_DB_pool.getConnection(async conn => conn)
    try {
        const hash = crypto.createHash('sha256')
        hash.update(pwd);
        let hash_password = hash.digest('hex') // 비밀번호 단방향 암호화

        const STRING = 'SELECT * FROM Auth WHERE id=? and pwd=?'; 

        const [DB] = await connection.query(STRING, [id, hash_password])
        console.log(DB)

        connection.release()

        if (Array.isArray(DB) && DB.length !== 0) {
            return done(null, DB)
        }
        else {
            return done(null, false)
        }
            
    } catch (err) {
        connection.release()
        return done(err)
    } 
}))

exports.isLocalAuthenticated = passport.authenticate('local', {
    failureRedirect: '/log_fail', // 로그인 실패 --> log_fail.GET Method로 이동
}), (req, res) => {
    req.session.save(() => { res.redirect('/') })}


