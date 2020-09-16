const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = (ENV) => {

    return merge(common(ENV), {
        mode: "production",
        // devtool: 'source-map' // Don't really need this source map
    })
}