const pool_k = require('../DB_open/open_OSSW_DB')

const crypto = require('crypto') // 비밀번호의 단방향 암호화

const id_crabz = /^(?=.*[a-z])(?=.*[0-9]).{5,20}$/; // 아이디 정규표현식

const pwd_crabz = /^(?=.*[a-zA-Z!@#$%])(?=.*[0-9]).{8,16}$/; // 비밀번호 정규표현식

exports.log_in = async (req, res) => {
    try {
        if (req.user === undefined) 
            return new Error('회원 정보 없음!')
        return res.send(req.user)
    } catch(err) {
        return res.status(400).send(err.message)
    }
    
}
exports.log_out = async (req, res) => {
    req.session.destroy( function ( err ) {
        return res.send( { message: 'Successfully logged out' } );
    });
}

exports.sign_up = async(req, res) => {
    try {

        if (!id_crabz.test(req.body.id))
            throw new Error("회원 가입 시 아이디는 영문 소문자 + 숫자 조합 5~20자 이내만 가능합니다.")
    
        if (!pwd_crabz.test(req.body.pwd))
            throw new Error("회원가입 시 비밀번호는 영문(대/소문자) + 숫자 + 특수문자 (!, @, #, $, %) 조합 8~16자 이내만 가능합니다.")

        if (req.body.pwd !== req.body.pwd_again)
            throw new Error("재입력 비밀번호가 틀립니다.")

        const connection = await pool_k.getConnection(async conn => conn)
        const hash = crypto.createHash('sha256');
        hash.update(req.body.pwd);
        let hash_password = hash.digest('hex');
        
        const [DB1] = await connection.query(`SELECT * FROM Auth where id = ?`, [req.body.id]);
        if (DB1.length !== 0)
            throw new Error("이미 ID가 있습니다.")

        const [DB2] = await connection.query('INSERT INTO Auth (id, pwd) values (?, ?)', [req.body.id, hash_password]);
        return res.send({status:200, result: "rr"});

    }
    catch(err){
        return res.status(400).send(err.message)
    }
    finally{

    }
}