const express = require('express');
const depreciation = require('../services/depreciation');
const router = new express.Router();
 
router.post('/', async (req, res, next) => {
  let options = { 
  };


  try {
    const result = await depreciation.calculateDepreciation(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;