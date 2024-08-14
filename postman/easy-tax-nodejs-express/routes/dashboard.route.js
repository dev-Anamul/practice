const express = require('express');
const dashboard = require('../services/dashboard');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await dashboard.getDashboardData(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/daily-expense', async (req, res, next) => {
  let options = { 
    "numOfDays": req.query.numOfDays,
  };


  try {
    const result = await dashboard.getDailyExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/monthly-expense', async (req, res, next) => {
  let options = { 
    "numOfMonths": req.query.numOfMonths,
  };


  try {
    const result = await dashboard.getMonthlyExpense(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;