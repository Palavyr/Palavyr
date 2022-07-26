const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = () => {
  const ENV = 'development';
  return merge(common(ENV), {
    entry: {
      'palavyr-pdf': './src/start_app.ts',
    },
    mode: 'development',
    devtool: 'inline-source-map',
  });
};
