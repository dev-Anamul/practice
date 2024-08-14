const express = require('express');
const fiscal_years = require('../services/fiscal_years');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
    "limit": req.query.limit,
    "page": req.query.page,
  };


  try {
    const result = await fiscal-years.getAllFiscalYears(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/', async (req, res, next) => {
  let options = { 
  };

  options.createFiscalYearInlineReqJson = req.body;

  try {
    const result = await fiscal-years.createFiscalYear(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await fiscal-years.getFiscalYearById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.delete('/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await fiscal-years.deleteFiscalYearById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.fiscalYearDto = req.body;

  try {
    const result = await fiscal-years.updateFiscalYearById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;