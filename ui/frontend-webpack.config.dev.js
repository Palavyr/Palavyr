const { merge } = require('webpack-merge');
const common = require('./frontend-webpack.common.js');

module.exports = (ENV) => {

    return merge(common(ENV), {
        mode: 'development',
        devtool: 'inline-source-map',
        devServer: {
            contentBase: './dist-frontend',
            historyApiFallback: true,
            hot: true,
            stats: 'errors-only',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "*",  //"GET, POST, PUT, DELETE, PATCH, OPTIONS",
                "Access-Control-Allow-Headers": "*"   //"X-Requested-With, content-type, Authorization"
            }
        },
    })

}
