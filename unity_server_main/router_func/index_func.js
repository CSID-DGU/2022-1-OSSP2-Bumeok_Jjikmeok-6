const pool_k = require('../DB_info/open_OSSW_DB')

const crypto = require('crypto') // 비밀번호의 단방향 암호화

const id_RE = /^(?=.*[a-z])(?=.*[0-9]).{5,20}$/ // 아이디 정규표현식

const pwd_RE = /^(?=.*[a-zA-Z!@#$%])(?=.*[0-9]).{8,16}$/ // 비밀번호 정규표현식

exports.log_in = async (req, res) => {
    try {
        return res.status(200).send({user_info : req.user[0], success_message: "로그인 성공!"})
    } catch(err) {
        return res.status(400).send(err.message)
    }
    
}
exports.log_out = async (req, res) => {
    req.session.destroy( function ( err ) {
        return res.status(200).send({message: 'Successfully logged out' })
    })
}

exports.sign_up = async(req, res) => {
    try {

        if (req.body.id.length === 0 || req.body.pwd.length === 0)
            throw new Error('아이디와 비밀번호 입력을 안 하셨네요!')
        if (!id_RE.test(req.body.id))
            throw new Error("회원 가입 시 아이디는 영문 소문자 + 숫자 조합 5~20자 이내만 가능합니다.")
    
        if (!pwd_RE.test(req.body.pwd))
            throw new Error("회원가입 시 비밀번호는 영문(대/소문자) + 숫자 + 특수문자 (!, @, #, $, %) 조합 8~16자 이내만 가능합니다.")

        if (req.body.pwd !== req.body.pwd_again)
            throw new Error("재입력 비밀번호가 틀립니다.")

        const connection = await pool_k.getConnection(async conn => conn)
        const hash = crypto.createHash('sha256')
        hash.update(req.body.pwd)
        let hash_password = hash.digest('hex')
        
        const [DB1] = await connection.query(`SELECT * FROM Auth where id = ?`, [req.body.id])
        if (DB1.length !== 0)
            throw new Error("이미 ID가 있습니다.")

        const [DB2] = await connection.query('INSERT INTO Auth (id, pwd) values (?, ?)', [req.body.id, hash_password])

        return res.status(200).send("회원가입 성공")

    } catch(err){
        return res.status(400).send(err.message)
    }
}

exports.Get_Rank = async(req, res) => {
    try {
        const connection = await pool_k.getConnection(async conn => conn)
        const [DB1] = await connection.query(`select id, score1, score2, score3 from auth left join ranking on auth.keycode = ranking.Auth_id`)

        if (DB1.length === 0)
            throw new Error("랭킹이 비었습니다.")

        return res.status(200).send({item: DB1})

    } catch(err){
        return res.status(400).send(err.message)
    }
}

exports.log_fail = async(req, res) => {
    return res.status(400).send('에러 : 회원 정보가 존재하지 않습니다')
}

exports.isNotLoggedIn = async(req, res, next) => {
    if (!req.isAuthenticated())
    {
        next()
    }
    else
    {
        return res.status(400).send("이미 로그인 했어요")
    }
}
exports.isLoggedIn = async(req, res, next) => {
    if (req.isAuthenticated())
    {
        next()
    }
    else
    {
        return res.status(400).send("로그인 없인 아무것도 할 수 없어요")
    }
}
exports.connect_check = async(req, res, next) => {
    return res.status(200).send({message: 'Successfully connected' })
}