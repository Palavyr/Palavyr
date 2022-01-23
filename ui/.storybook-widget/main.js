const commonWebpack = require("../widget-webpack.common");

module.exports = {
    stories: ["../src/**/*.stories.mdx", "../src/**/*.stories.@(js|jsx|ts|tsx)"],
    addons: ["@storybook/addon-links", "@storybook/addon-essentials"],
    webpackFinal: async config => {
        const wp = commonWebpack("Storybook Mode!");
        return {
            ...config,
            resolve: { ...config.resolve, ...wp.resolve },
            module: { ...config.module, rules: [...config.module.rules, ...wp.module.rules] },
        };
    },
};
