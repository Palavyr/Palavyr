const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = () => {
    const ENV = 'production';

    return merge(common(ENV), {
        entry: {
            'palavyr-pdf': './src/start_app.ts',
        },
        mode: 'production',
        devtool: 'inline-source-map',
    });
};
