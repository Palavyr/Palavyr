const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = () => {
    const ENV = 'development';
    return merge(common(ENV), {
        mode: 'development',
        devtool: 'inline-source-map',
    });
};
