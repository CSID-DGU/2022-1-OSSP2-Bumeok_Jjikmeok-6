const pool_k = require('../DB_info/open_OSSW_DB')

const crypto = require('crypto') // 비밀번호의 단방향 암호화
const { debug } = require('console')

const id_RE = /^(?=.*[a-z])(?=.*[0-9]).{5,20}$/ // 아이디 정규표현식

const pwd_RE = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,16}/ // 비밀번호 정규표현식

exports.log_in = async (req, res) => {
    try {
        return res.status(200).send({user_info : req.user[0], success_message: "로그인 성공!"})
    } catch(err) {
        return res.status(400).send(err.message)
    }
    
}
exports.log_out = async (req, res) => {
    req.session.destroy( function ( err ) {
        res.locals.user = req.user;
        return res.status(200).send({message: 'Successfully logged out' })
    })
}

exports.sign_up = async(req, res) => {
    try
    {
        const connection = await pool_k.getConnection(async conn => conn)
        try {

            if (req.body.id.length === 0 || req.body.pwd.length === 0)
                throw new Error('아이디와 비밀번호 입력을 안 했습니다')
            if (!id_RE.test(req.body.id))
                throw new Error("회원 가입 시 아이디는 영문 소문자 + 숫자 조합 5~20자 이내만 가능합니다")
        
            if (!pwd_RE.test(req.body.pwd))
                throw new Error("회원가입 시 비밀번호는 영문(대/소문자) + 숫자 + 특수문자 조합 8~16자 이내만 가능합니다")
    
            if (req.body.pwd !== req.body.pwd_again)
                throw new Error("재입력 비밀번호가 틀립니다")
    
            const hash = crypto.createHash('sha256')
            hash.update(req.body.pwd)
            let hash_password = hash.digest('hex')
            
            const [DB1] = await connection.query(`SELECT * FROM Auth where id = ?`, [req.body.id])
            if (DB1.length !== 0)
                throw new Error("이미 ID가 있습니다")
    
            const [DB2] = await connection.query('INSERT INTO Auth (id, pwd) values (?, ?)', [req.body.id, hash_password])
    
            connection.release()
            return res.status(200).send("회원가입 성공")
    
        } catch(err){
            connection.release()
            return res.status(400).send(err.message)
        }
    }  catch(err){
        console.log("DB Error")
        return res.status(400).send(err.message)
    }
}

exports.Set_Rank = async(req, res) => {
    try
    {
        const connection = await pool_k.getConnection(async conn => conn)
        try {
            req.body.main_stage_1_score = parseInt(req.body.main_stage_1_score);
            req.body.main_stage_2_score = parseInt(req.body.main_stage_2_score);
            req.body.main_stage_3_score = parseInt(req.body.main_stage_3_score);
            req.body.final_stage_1_score = parseInt(req.body.final_stage_1_score);
            req.body.final_stage_2_score = parseInt(req.body.final_stage_2_score);
            if (req.body.main_stage_1_score === 0 && req.body.main_stage_2_score === 0
                && req.body.main_stage_3_score === 0 && req.body.final_stage_1_score === 0 &&
                req.body.final_stage_2_score === 0)
                throw new Error('스테이지의 기록이 모두 0이면 랭킹 등록이 안됩니다')
    
            const [DB_match_Auth] = await connection.query(`select keycode from Auth where id=?`, [req.user])
    
            if (DB_match_Auth[0].keycode <= 0)
                throw new Error("올바르지 않은 DB 입력입니다")
    
            const [DB_Old] = await connection.query(`select id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2 
            from Auth left join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
    
            const [DB_new_Insert] = await connection.query(`insert into Ranking (Auth_id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2) values (?, ?, ?, ?, ?, ?)
            on duplicate key update Auth_id=?, main_score_1=?, main_score_2=?, main_score_3=?, final_score_1=?, final_score_2=?`,
            [DB_match_Auth[0].keycode, req.body.main_stage_1_score, req.body.main_stage_2_score, 
            req.body.main_stage_3_score, req.body.final_stage_1_score, req.body.final_stage_2_score,
            DB_match_Auth[0].keycode, req.body.main_stage_1_score, req.body.main_stage_2_score, 
            req.body.main_stage_3_score, req.body.final_stage_1_score, req.body.final_stage_2_score])
    
            const [DB_New] = await connection.query(`select id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2 
            from Auth left join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
    
            connection.release()
            return res.status(200).send({old_rank : DB_Old, new_rank : DB_New, successful_message : "랭킹 반영 성공"})
    
        } catch(err){
            connection.release()
            return res.status(400).send(err.message)
        }
    } catch(err){
        console.log("DB Error")
        return res.status(400).send(err.message)
    }
}
exports.Get_Rank_Total_Login = async(req, res) => {
    try
    {
        const connection = await pool_k.getConnection(async conn => conn)
        try {
            const [DB_match_Auth] = await connection.query(`select keycode from Auth where id=?`, [req.user])
    
            if (DB_match_Auth[0].keycode <= 0)
                throw new Error("올바르지 않은 DB 입력입니다")
            
            const [DB_Mine] = await connection.query(`select id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
            
            const [DB_Total] = await connection.query(`select id, main_score_1, main_score_2, main_score_3, final_score_1, final_score_2 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id`)
    
            connection.release()
            return res.status(200).send({rank_mine: DB_Mine, rank_total: DB_Total})
    
        } catch(err){
            connection.release()
            return res.status(400).send(err.message)
        }
    }  catch(err){
        console.log("DB Error")
        return res.status(400).send(err.message)
    }
    
}
exports.Get_Rank_Detail_Login = async(req, res, next) => {
    try
    {
        const connection = await pool_k.getConnection(async conn => conn)
        try {
            console.log(req.body.identifier)
    
            const [DB_match_Auth] = await connection.query(`select keycode from Auth where id=?`, [req.user])

            if (DB_match_Auth[0].keycode <= 0)
                throw new Error("올바르지 않은 DB 입력입니다")
    
            let [DB_Mine] = []
            let [DB_Total] = []
            if (parseInt(req.body.identifier) === 0)
            {
                DB_Mine = await connection.query(`select id, main_score_1 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
                DB_Total = await connection.query(`select id, main_score_1 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id order by main_score_1 ASC`)
            }
            if (parseInt(req.body.identifier) === 1)
            {
                DB_Mine = await connection.query(`select id, main_score_2 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
                DB_Total = await connection.query(`select id, main_score_2 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id order by main_score_2 ASC`)
            }
            if (parseInt(req.body.identifier) === 2)
            {
                DB_Mine = await connection.query(`select id, main_score_3 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
                DB_Total = await connection.query(`select id, main_score_3 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id order by main_score_3 DESC`)
            }
            if (parseInt(req.body.identifier) === 3)
            {
                DB_Mine = await connection.query(`select id, final_score_1 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
                DB_Total = await connection.query(`select id, final_score_1 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id order by final_score_1 ASC`)
            }
            if (parseInt(req.body.identifier) === 4)
            {
                DB_Mine = await connection.query(`select id, final_score_2 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id where id=?`, [req.user])
                DB_Total = await connection.query(`select id, final_score_2 from Auth inner join Ranking on Auth.keycode = Ranking.Auth_id order by final_score_2 ASC`)
            }

            console.log(DB_Total.length)
    
            connection.release()
            return res.status(200).send({rank_mine : DB_Mine[0], rank_total: DB_Total[0]})
    
        } catch(err){
            connection.release()
            return res.status(400).send(err.message)
        }

    }  catch(err){
        console.log("DB Error")
        return res.status(400).send(err.message)
    }
   
}
exports.log_fail = async(req, res) => {
    return res.status(400).send("회원 정보가 존재하지 않습니다")
}

exports.isNotLoggedIn = async(req, res, next) => {
    if (!req.isAuthenticated())
    {
        next()
    }
    else
    {
        return res.status(400).send("이미 로그인 했습니다")
    }
}
exports.isLoggedIn = async(req, res, next) => {
    if (req.isAuthenticated())
    {
        next()
    }
    else
    {
        return res.status(400).send("로그인 없이는 아무것도 할 수 없습니다")
    }
}

exports.connect_check = async(req, res, next) => {
    try
    {
        const connection = await pool_k.getConnection(async conn => conn)
        try {
            const [DB_match_Auth] = await connection.query(`select keycode from Auth where id=?`, [req.user])
        
            if (DB_match_Auth[0].keycode <= 0)
                throw new Error("계정 정보가 올바르지 않습니다")
    
            connection.release()
            return res.status(200).send({user_info: req.user, message: 'Successfully Connected' })
        }
        catch { 
            connection.release()
            return res.status(400).send(err.message)
        }
    }  catch(err){
        console.log("DB Error")
        return res.status(400).send(err.message)
    }
}