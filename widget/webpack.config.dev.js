'use strict'

const webpack = require('webpack');
const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const ManifestPlugin = require('webpack-manifest-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const { manifestOptions, htmlOptions } = require(path.resolve(__dirname, './webpack/options'));


const common = require('./webpack.common.js');
const { merge } = require('webpack-merge');

process.env.NODE_ENV = 'development';

module.exports = (ENV) => {
  const envPath = ENV.production ? ".env.production" : ".env.development";

  return merge(common(ENV), {
    mode: 'development',
    devServer: {
      contentBase: path.resolve(__dirname, 'dist'),
      historyApiFallback: true,
      compress: false,
      port: 3400,
      hot: true
    },
    devtool: 'inline-source-map',
    plugins: [
      new webpack.HotModuleReplacementPlugin(),

      new CleanWebpackPlugin(),
      new MiniCssExtractPlugin({
        filename: 'styles.css',
        chunkFileName: '[id].css'
      }),
      new Dotenv({ path: envPath }),
      new ManifestPlugin(manifestOptions),
      new HtmlWebpackPlugin(htmlOptions),
      new ForkTsCheckerWebpackPlugin(),


      new webpack.ProvidePlugin({
        'React': 'react'
      })
    ],
    performance: {
      hints: false
    }
  })
}
