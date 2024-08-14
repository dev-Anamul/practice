const express = require('express');
const notifications = require('../services/notifications');
const router = new express.Router();
 
router.get('/', async (req, res, next) => {
  let options = { 
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await notifications.getNotifications(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/channels', async (req, res, next) => {
  let options = { 
    "fields": req.query.fields,
    "limit": req.query.limit,
    "order": req.query.order,
    "page": req.query.page,
    "search": req.query.search,
    "sort": req.query.sort,
  };


  try {
    const result = await notifications.getAllNotificationChannels(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/channels', async (req, res, next) => {
  let options = { 
  };

  options.addNotificationChannelInlineReqFormdata = req.body;

  try {
    const result = await notifications.addNotificationChannel(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/channels/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await notifications.getNotificationChannelById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.delete('/channels/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await notifications.deleteNotificationChannelById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/channels/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.updateNotificationChannelByIdInlineReqFormdata = req.body;

  try {
    const result = await notifications.updateNotificationChannelById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/channels/:id/notify', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.notifyChannelUsersInlineReqFormdata = req.body;

  try {
    const result = await notifications.notifyChannelUsers(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.get('/channels/:id/users', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };


  try {
    const result = await notifications.getChannelUsers(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/channels/:id/users', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.addUsersToChannelInlineReqJson = req.body;

  try {
    const result = await notifications.addUsersToChannel(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.delete('/channels/:id/users/:userId', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
    "userId": req.params.userId,
  };


  try {
    const result = await notifications.removeUserFromChannel(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/users/all', async (req, res, next) => {
  let options = { 
  };

  options.sendNotificationToAllUsersInlineReqFormdata = req.body;

  try {
    const result = await notifications.sendNotificationToAllUsers(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/users/many', async (req, res, next) => {
  let options = { 
  };

  options.sendNotificationToManyUsersInlineReqFormdata = req.body;

  try {
    const result = await notifications.sendNotificationToManyUsers(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.post('/users/:id', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.createNotificationInlineReqFormdata = req.body;

  try {
    const result = await notifications.createNotification(options);
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
    const result = await notifications.getNotificationById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});
 
router.patch('/:id/read', async (req, res, next) => {
  let options = { 
    "id": req.params.id,
  };

  options.updateNotificationReadStatusByIdInlineReqJson = req.body;

  try {
    const result = await notifications.updateNotificationReadStatusById(options);
    res.status(result.status || 200).send(result.data);
  }
  catch (err) {
    return res.status(500).send({
      error: err || 'Something went wrong.'
    });
  }
});

module.exports = router;