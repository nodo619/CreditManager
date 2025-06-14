const express = require('express');
const history = require('connect-history-api-fallback');
const path = require('path');

const app = express();
const port = 1841;

// Enable history API fallback
app.use(history());

// Serve static files from the classic build directory
app.use(express.static(path.join(__dirname, 'build', 'development', 'CreditManager.UI', 'classic')));

// Handle all routes by serving index.html
app.get('*', (req, res) => {
    res.sendFile(path.join(__dirname, 'build', 'development', 'CreditManager.UI', 'classic', 'index.html'));
});

app.listen(port, () => {
    console.log(`Server running at http://localhost:${port}`);
}); 