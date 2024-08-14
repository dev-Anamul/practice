const express = require('express');
const auth = require('../services/auth');
const router = new express.Router();
 
router.delete('/delete-profile', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await auth.deleteProfile(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/forgot-password', async (req, res, next) => {
  let options = { 
  };

  options.forgotPasswordInlineReqJson = req.body;

  try {
    const result = await auth.forgotPassword(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/login', async (req, res, next) => {
  let options = { 
  };

  options.loginInlineReqJson = req.body;

  try {
    const result = await auth.login(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/profile', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await auth.getProfile(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/reset-password', async (req, res, next) => {
  let options = { 
  };

  options.resetPasswordInlineReqJson = req.body;

  try {
    const result = await auth.resetPassword(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/signup', async (req, res, next) => {
  let options = { 
  };

  options.signupInlineReqJson = req.body;

  try {
    const result = await auth.signup(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/update-password', async (req, res, next) => {
  let options = { 
  };

  options.updatePasswordInlineReqJson = req.body;

  try {
    const result = await auth.updatePassword(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/update-profile', async (req, res, next) => {
  let options = { 
  };

  options.updateProfileInlineReqFormdata = req.body;

  try {
    const result = await auth.updateProfile(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;