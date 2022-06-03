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
    done(null, user[0].id); // 여기서의 user[0].email은 deserializeUser의 id로 전달된다.
})
    
passport.deserializeUser((id, done) => { // 페이지를 접근할 때마다 호출되는 것
    console.log('deserializeUser : ', id)
    done(null, id); // id는 request.user로 저장되고 log_in (GET)으로 이동
})

router.route('/log_in')
.post(router_func.isNotLoggedIn, Auth.isLocalAuthenticated, router_func.log_in)

router.route('/log_out')
.get(router_func.isLoggedIn, router_func.log_out)

router.route('/ssibal')
.get(router_func.ssibal)

router.route('/sign_up')
.post(router_func.isNotLoggedIn, router_func.sign_up)

router.route('/page_up')
.get(router_func.page_up)

router.route('/Get_Rank')
.get(router_func.Get_Rank)

router.route('/Get_Rank_For_Main_1')
.get(router_func.Get_Rank_For_Main_1)

router.route('/Get_Rank_For_Main_2')
.get(router_func.Get_Rank_For_Main_2)

router.route('/Get_Rank_For_Main_3')
.get(router_func.Get_Rank_For_Main_3)

router.route('/Get_Rank_For_Final_4')
.get(router_func.Get_Rank_For_Final_1)

router.route('/Get_Rank_For_Final_5')
.get(router_func.Get_Rank_For_Final_2)

router.route('/Set_Rank')
.post(router_func.isLoggedIn, router_func.Set_Rank)

router.route('/log_fail')
.get(router_func.log_fail)

router.route('/continue_connect')
.get(router_func.isLoggedIn, router_func.connect_check)


module.exports = router