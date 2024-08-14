const express = require('express');
const tax = require('../services/tax');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
    "years": req.query.years,
  };


  try {
    const result = await tax.getListOfTaxes(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/users/:userId', async (req, res, next) => {
  let options = { 
    "userId": req.params.userId,
    "years": req.query.years,
  };


  try {
    const result = await tax.getUserTaxes(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;