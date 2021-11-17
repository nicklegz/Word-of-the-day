const express = require('express');
const path = require('path');

const app = express();

// Serve only the static files form the dist directory
app.use(express.static(__dirname + '/dist/word-of-the-day'));

app.use((req, res, next) => {
    if (req.header('x-forwarded-proto') !== 'https')
      res.redirect(`https://${req.header('host')}${req.url}`)
    else
      next()
})

app.get('/', function(req,res) {
    
res.sendFile(path.join(__dirname+'/dist/word-of-the-day/index.html'));
});

// Start the app by listening on the default Heroku port
app.listen(process.env.PORT || 8080);
