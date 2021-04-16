const path = require("path");
const TsconfigPathsPlugin = require("tsconfig-paths-webpack-plugin");
const ManifestPlugin = require('webpack-manifest-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const Dotenv = require('dotenv-webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CleanWebpackPlugin = require("clean-webpack-plugin");

const webpack = require("webpack");

const { createRules } = require("../webpack/rules");
const { manifestOptions, htmlOptions } = require(path.resolve(__dirname, '../webpack/options'));


const commonWebpack = require("../webpack.common");

module.exports = {
    stories: ["../src/**/*.stories.mdx", "../src/**/*.stories.@(js|jsx|ts|tsx)"],
    addons: ["@storybook/addon-links", "@storybook/addon-essentials"],
    webpackFinal: async (config, { configType }) => {
        const wp = commonWebpack("Storybook Mode!");
        return {
            ...config,
            resolve: { ...config.resolve, ...wp.resolve },
            module: { ...config.module, rules: [...config.module.rules, ...wp.module.rules] },
        };
    },
};
