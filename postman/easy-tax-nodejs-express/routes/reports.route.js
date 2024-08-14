const express = require('express');
const reports = require('../services/reports');
const router = new express.Router();
 
router.get('/gst-report', async (req, res, next) => {
  let options = { 
    "end": req.query.end,
    "start": req.query.start,
    "userId": req.query.userId,
  };


  try {
    const result = await reports.getGstReport(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;