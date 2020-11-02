'use strict'

const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const { manifestOptions, htmlOptions } = require(path.resolve(__dirname, './webpack/options'));
const common = require('./webpack.common.js');
const { merge } = require('webpack-merge');
const ManifestPlugin = require('webpack-manifest-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');


process.env.NODE_ENV = 'production';

module.exports = (ENV) => {
  const envPath = ENV.production ? ".env.production" : ".env.development";

  return merge(common(ENV), {
    mode: 'production',
    plugins: [
      new CleanWebpackPlugin(),
      new MiniCssExtractPlugin({
        filename: 'styles.css',
        chunkFileName: '[id].css'
      }),
      new Dotenv({ path: envPath }),
      new ManifestPlugin(manifestOptions),
      new HtmlWebpackPlugin(htmlOptions),
      new ForkTsCheckerWebpackPlugin(),

    ],
    externals: {
      react: {
        commonjs: 'React',
        commonjs2: 'react',
        amd: 'react'
      },
      'react-dom': {
        commonjs: 'ReactDOM',
        commonjs2: 'react-dom',
        amd: 'react-dom'
      }
    },
    optimization: {
      minimizer: [
        new UglifyJsPlugin({
          cache: true,
          parallel: true
        }),
        new OptimizeCSSAssetsPlugin({})
      ]
    }
  })
}
