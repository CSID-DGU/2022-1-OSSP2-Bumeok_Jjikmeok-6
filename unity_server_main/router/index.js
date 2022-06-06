const express = require('express')

const router = express.Router()

const router_func = require('../router_func/index_func')

const passport = require('passport')

const cors = require('cors')

const cookieParser = require('cookie-parser')

const session = require('express-session')

let flash = require('connect-flash')

const Auth = require('../Auth/Authentication_with_passport')

router.use(cors({
    origin: "http://localhost:3000",
    credentials: true
}))
  
router.use(session({
    secret: 'asadlfkj!@#!@#dfgasdg',
    resave: false,
    saveUninitialized: false
}))
  
router.use(flash())
router.use(cookieParser("secretcode"))
router.use(passport.initialize())
router.use(passport.session())
  
passport.serializeUser((user, done) => {
    console.log('serializeUser : ', user)
    done(null, user[0].id); // 여기서의 user[0].id는 deserializeUser의 id로 전달된다.
})
    
passport.deserializeUser((id, done) => { // 페이지를 접근할 때마다 호출되는 것
    console.log('deserializeUser : ', id)
    done(null, id); // id는 request.user로 저장되고 log_in (GET)으로 이동
})

router.route('/log_in')
.post(router_func.isNotLoggedIn, Auth.isLocalAuthenticated, router_func.log_in)

router.route('/log_out')
.get(router_func.isLoggedIn, router_func.log_out)

router.route('/sign_up')
.post(router_func.isNotLoggedIn, router_func.sign_up)

router.route('/get_rank_total_login')
.get(router_func.isLoggedIn, router_func.Get_Rank_Total_Login)

router.route('/get_rank_total_not_login')
.get(router_func.Get_Rank_Total_Not_Login)

router.route('/set_rank')
.post(router_func.isLoggedIn, router_func.Set_Rank)

router.route('/get_rank_detail_login')
.post(router_func.isLoggedIn, router_func.Get_Rank_Detail_Login)

router.route('/get_rank_detail_not_login')
.post(router_func.Get_Rank_Detail_Not_Login)

router.route('/log_fail')
.get(router_func.log_fail)

router.route('/continue_connect')
.get(router_func.isLoggedIn, router_func.connect_check)

module.exports = router