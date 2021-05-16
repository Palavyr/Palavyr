const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = (ENV) => {

    return merge(common(ENV), {
        mode: 'production',
        devtool: 'inline-source-map',
    })

}
