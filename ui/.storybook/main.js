const TsconfigPathsPlugin = require('tsconfig-paths-webpack-plugin');

module.exports = {
    "stories": [
        "../src/**/*.stories.mdx",
        "../src/**/*.stories.@(js|jsx|ts|tsx)"
    ],
    "addons": [
        "@storybook/addon-essentials",

    ],

    webpackFinal: async (config, { configType }) => {
        config.resolve.plugins.push(new TsconfigPathsPlugin())
        return config;
    }

}
