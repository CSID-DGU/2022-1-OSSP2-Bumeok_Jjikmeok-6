const express = require('express')

const router = express.Router()

const kuku = require('../router_func/index_func')

const passport = require('passport');

const cors = require('cors');

const cookieParser = require('cookie-parser');

const session = require('express-session');

let flash = require('connect-flash');

const Auth = require('../Auth/Authentication_with_passport');

router.use(cors({
    origin: "http://localhost:3000",
    credentials: true
}));
  
router.use(session({
    secret: 'asadlfkj!@#!@#dfgasdg',
    resave: false,
    saveUninitialized: false
}));
  
router.use(flash());
router.use(cookieParser("secretcode"));
router.use(passport.initialize());
router.use(passport.session());
  
passport.serializeUser((user, done) => {
    console.log('serializeUser : ', user);
    done(null, user[0].id); // 여기서의 user[0].email은 deserializeUser의 id로 전달된다.
});
    
passport.deserializeUser((id, done) => { // 페이지를 접근할 때마다 호출되는 것
    console.log('deserializeUser : ', id);
    done(null, id); // 이 id라는 새끼가 request.user로 가는 거다.
});

router.route('/log_in')
.post(Auth.isLocalAuthenticated, kuku.log_in)


router.route('/log_out')
.get(kuku.log_out)

router.route('/sign_up')
.post(kuku.sign_up)


module.exports = router;