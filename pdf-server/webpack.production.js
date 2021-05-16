const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = () => {
    const ENV = "production";

    return merge(common(ENV), {
        mode: 'production',
        devtool: 'inline-source-map',
    })

}
