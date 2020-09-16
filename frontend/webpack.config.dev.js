const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = (ENV) => {

    return merge(common(ENV), {
        mode: 'development',
        devtool: 'inline-source-map',
        devServer: {
            contentBase: './dist',
            historyApiFallback: true,
            hot: true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "GET, POST, PUT, DELETE, PATCH, OPTIONS",
                "Access-Control-Allow-Headers": "X-Requested-With, content-type, Authorization"
            }
        },
    })

}
