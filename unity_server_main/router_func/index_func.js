const pool_k = require('../DB_open/open_OSSW_DB')

const crypto = require('crypto') // 비밀번호의 단방향 암호화

const CustorError = require('custom-error');

const id_crabz = /^(?=.*[a-zA-Z])(?=.*[0-9]).{6,12}$/; // 아이디 정규표현식

const pwd_crabz = /^(?=.*[a-zA-Z!@#$%])(?=.*[0-9]).{6,20}$/; // 비밀번호 정규표현식

exports.log_in = async (req, res) => {
    if (req.user === undefined) 
        return res.send('null')
    return res.send(req.user)
}
exports.log_out = async (req, res) => {
    req.session.destroy( function ( err ) {
        return res.send( { message: 'Successfully logged out' } );
    });
}
exports.failure = async (req, res) => {
    return res.send('null')
}

exports.sign_up = async(req, res) => {
    try {

        if (!id_crabz.test(req.body.id))
            throw new Error("회원 가입 시 아이디는 영문 _ 숫자 6~12자 이내만 가능합니다.")
    
        if (!pwd_crabz.test(req.body.pwd))
            throw new Error("회원가입 시 비밀번호는 영문 + 숫자 + 비밀번호 6~20자 이내만 가능합니다.")

        if (req.body.pwd !== req.body.pwd_again)
            throw new Error("재입력 비밀번호 틀려서 회원가입 안 됩니다.")

        const connection = await pool_k.getConnection(async conn => conn)
        const hash = crypto.createHash('sha256');
        hash.update(req.body.pwd);
        let hash_password = hash.digest('hex');
        
        const [DB1] = await connection.query(`SELECT * FROM Auth where id = ?`, [req.body.id]);
        if (DB1.length !== 0)
            throw new Error("음.....너 병신?")

        const [DB2] = await connection.query('INSERT INTO Auth (id, pwd) values (?, ?)', [req.body.id, hash_password]);
        return res.send({status:200, result: "rr"});
        

    }
    catch(err){
        return res.status(400).send(err.message)
    }
    finally{

    }
}