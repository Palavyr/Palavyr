const { merge } = require('webpack-merge');
const common = require('./frontend-webpack.common.js');

module.exports = (_) => {

    return merge(common('development'), {
        mode: 'development',
        devtool: 'inline-source-map',
        devServer: {
            static: './dist-frontend',
            historyApiFallback: true,
            hot: true,
            client: {
                overlay: {
                    errors: true,
                    warnings: false,
                },
            },
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "*",  //"GET, POST, PUT, DELETE, PATCH, OPTIONS",
                "Access-Control-Allow-Headers": "*",  //"X-Requested-With, content-type, Authorization"
            },
        },
    })

}
