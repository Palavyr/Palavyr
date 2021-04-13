const devWebpack = require("../webpack.config.dev");
const prodWebpack = require("../webpack.config.prod");
const commonWebpack = require("../webpack.common");
const { common } = require("@material-ui/core/colors");

module.exports = {
    stories: ["../src/**/*.stories.mdx", "../src/**/*.stories.@(js|jsx|ts|tsx)"],
    addons: ["@storybook/addon-links", "@storybook/addon-essentials"],
    webpackFinal: async (config, { configType }) => {
        const wp = commonWebpack({});
        return { ...config, resolve: { ...config.resolve, ...wp.resolve }, module: { ...config.module, rules: [...config.module.rules, ...wp.module.rules] } };
    },
};
