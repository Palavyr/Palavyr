const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const fs = require('fs');
const postcssNormalize = require('postcss-normalize');
const getCSSModuleLocalIdent = require('react-dev-utils/getCSSModuleLocalIdent');


const TypeScriptLoaderRule = () => {
    return {
        test: /\.tsx?$/,
        use: [
            { loader: 'ts-loader', options: { transpileOnly: true } },
        ],
        exclude: /node_modules/
    }
}


const SVGRLoader = () => ({ test: /\.svg$/, use: ['@svgr/webpack'] })

const URLLoader = () => ({ test: /\.svg$/, loader: 'url-loader' })
const OtherUrlLoader = () => {
    return {
        test: [/\.avif$/],
        loader: require.resolve('url-loader'),
        options: {
            limit: imageInlineSizeLimit,
            mimetype: 'image/avif',
            name: 'static/media/[name].[hash:8].[ext]',
        },
    }
}


const FileLoader = () => {
    // "file" loader makes sure those assets get served by WebpackDevServer.
    // When you `import` an asset, you get its (virtual) filename.
    // In production, they would get copied to the `build` folder.
    // This loader doesn't use a "test" so it will catch all modules
    // that fall through the other loaders.
    return {
        loader: require.resolve('file-loader'),
        // Exclude `js` files to keep "css" loader working as it injects
        // its runtime that would otherwise be processed through "file" loader.
        // Also exclude `html` and `json` extensions so they get processed
        // by webpacks internal loaders.
        exclude: [/\.(js|mjs|jsx|ts|tsx)$/, /\.html$/, /\.json$/],
        options: {
            name: 'static/media/[name].[hash:8].[ext]',
        },
    }
}

const ImageLoader = () => {
    return {
        test: [/\.bmp$/, /\.gif$/, /\.jpe?g$/, /\.png$/],
        loader: require.resolve('url-loader'),
        options: {
            limit: imageInlineSizeLimit,
            name: 'static/media/[name].[hash:8].[ext]',
        },
    }
}

const BabelLoader = () => {
    return {
        test: /\.(js|mjs|jsx|ts|tsx)$/,
        include: paths.appSrc,
        loader: require.resolve('babel-loader'),
        options: {
            customize: require.resolve(
                'babel-preset-react-app/webpack-overrides'
            ),

            plugins: [
                [
                    require.resolve('babel-plugin-named-asset-import'),
                    {
                        loaderMap: {
                            svg: {
                                ReactComponent:
                                    '@svgr/webpack?-svgo,+titleProp,+ref![path]',
                            },
                        },
                    },
                ],
                isEnvDevelopment &&
                shouldUseReactRefresh &&
                require.resolve('react-refresh/babel'),
            ].filter(Boolean),
            // This is a feature of `babel-loader` for webpack (not Babel itself).
            // It enables caching results in ./node_modules/.cache/babel-loader/
            // directory for faster rebuilds.
            cacheDirectory: true,
            // See #6846 for context on why cacheCompression is disabled
            cacheCompression: false,
            compact: isEnvProduction,
        },
    }
}

const OtherBabelLoader = () => {
    return {
        test: /\.(js|mjs)$/,
        exclude: /@babel(?:\/|\\{1,2})runtime/,
        loader: require.resolve('babel-loader'),
        options: {
            babelrc: false,
            configFile: false,
            compact: false,
            presets: [
                [
                    require.resolve('babel-preset-react-app/dependencies'),
                    { helpers: true },
                ],
            ],
            cacheDirectory: true,
            // See #6846 for context on why cacheCompression is disabled
            cacheCompression: false,

            // Babel sourcemaps are needed for debugging into node_modules
            // code.  Without the options below, debuggers like VSCode
            // show incorrect code and set breakpoints on the wrong lines.
            sourceMaps: shouldUseSourceMap,
            inputSourceMap: shouldUseSourceMap,
        },
    }
}

const CSSLoader = () => {
    return {
        test: cssRegex,
        exclude: cssModuleRegex,
        use: getStyleLoaders({
            importLoaders: 1,
            sourceMap: isEnvProduction
                ? shouldUseSourceMap
                : isEnvDevelopment,
        }),
        // Don't consider CSS imports dead code even if the
        // containing package claims to have no side effects.
        // Remove this when webpack adds a warning or an error for this.
        // See https://github.com/webpack/webpack/issues/6571
        sideEffects: true,
    },
}

const CreateReactAppRules = (isProduction) => {

    const cssRegex = /\.css$/;
    const cssModuleRegex = /\.module\.css$/;
    const sassRegex = /\.(scss|sass)$/;
    const sassModuleRegex = /\.module\.(scss|sass)$/;

    const imageInlineSizeLimit = '10000';

    const appDirectory = fs.realpathSync(process.cwd());
    const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

    const paths = {
        appSrc: resolveApp('src')
    }

    const isEnvDevelopment = isProduction ? false : true;
    const isEnvProduction = !isEnvDevelopment;

    const shouldUseReactRefresh = true; //env.raw.FAST_REFRESH;
    const shouldUseSourceMap = isEnvDevelopment;

    process.env.BABEL_ENV = isEnvDevelopment ? 'development' : 'production';
    process.env.NODE_ENV = isEnvDevelopment ? 'development' : 'production';

    const getStyleLoaders = (cssOptions, preProcessor) => {
        const loaders = [
            isEnvDevelopment && require.resolve('style-loader'),
            isEnvProduction && {
                loader: MiniCssExtractPlugin.loader,
                // css is located in `static/css`, use '../../' to locate index.html folder
                // in production `paths.publicUrlOrPath` can be a relative path
                options: paths.publicUrlOrPath.startsWith('.')
                    ? { publicPath: '../../' }
                    : {},
            },
            {
                loader: require.resolve('css-loader'),
                options: cssOptions,
            },
            {
                // Options for PostCSS as we reference these options twice
                // Adds vendor prefixing based on your specified browser support in
                // package.json
                loader: require.resolve('postcss-loader'),
                options: {
                    // Necessary for external CSS imports to work
                    // https://github.com/facebook/create-react-app/issues/2677
                    ident: 'postcss',
                    plugins: () => [
                        require('postcss-flexbugs-fixes'),
                        require('postcss-preset-env')({
                            autoprefixer: {
                                flexbox: 'no-2009',
                            },
                            stage: 3,
                        }),
                        // Adds PostCSS Normalize as the reset css with default options,
                        // so that it honors browserslist config in package.json
                        // which in turn let's users customize the target behavior as per their needs.
                        postcssNormalize(),
                    ],
                    sourceMap: isEnvProduction ? shouldUseSourceMap : isEnvDevelopment,
                },
            },
        ].filter(Boolean);
        if (preProcessor) {
            loaders.push(
                {
                    loader: require.resolve('resolve-url-loader'),
                    options: {
                        sourceMap: isEnvProduction ? shouldUseSourceMap : isEnvDevelopment,
                        root: paths.appSrc,
                    },
                },
                {
                    loader: require.resolve(preProcessor),
                    options: {
                        sourceMap: true,
                    },
                }
            );
        }
        return loaders;
    };

    return {
        rules: [
            // Disable require.ensure as it's not a standard language feature.
            { parser: { requireEnsure: false } },
            {
                // "oneOf" will traverse all following loaders until one will
                // match the requirements. When no loader matches it will fall
                // back to the "file" loader at the end of the loader list.
                oneOf: [
                    TypeScriptLoaderRule(),
                    URLLoader(),
                    // TODO: Merge this config once `image/avif` is in the mime-db
                    // https://github.com/jshttp/mime-db
                    OtherUrlLoader(),
                    // "url" loader works like "file" loader except that it embeds assets
                    // smaller than specified limit in bytes as data URLs to avoid requests.
                    // A missing `test` is equivalent to a match.
                    ImageLoader(),
                    // Process application JS with Babel.
                    // The preset includes JSX, Flow, TypeScript, and some ESnext features.
                    BabelLoader(),
                    // Process any JS outside of the app with Babel.
                    // Unlike the application JS, we only compile the standard ES features.
                    OtherBabelLoader(),
                    // "postcss" loader applies autoprefixer to our CSS.
                    // "css" loader resolves paths in CSS and adds assets as dependencies.
                    // "style" loader turns CSS into JS modules that inject <style> tags.
                    // In production, we use MiniCSSExtractPlugin to extract that CSS
                    // to a file, but in development "style" loader enables hot editing
                    // of CSS.
                    // By default we support CSS Modules with the extension .module.css
                    CSSLoader(),
                    // Adds support for CSS Modules (https://github.com/css-modules/css-modules)
                    // using the extension .module.css
                    {
                        test: cssModuleRegex,
                        use: getStyleLoaders({
                            importLoaders: 1,
                            sourceMap: isEnvProduction
                                ? shouldUseSourceMap
                                : isEnvDevelopment,
                            modules: {
                                getLocalIdent: getCSSModuleLocalIdent,
                            },
                        }),
                    },
                    // Opt-in support for SASS (using .scss or .sass extensions).
                    // By default we support SASS Modules with the
                    // extensions .module.scss or .module.sass
                    {
                        test: sassRegex,
                        exclude: sassModuleRegex,
                        use: getStyleLoaders(
                            {
                                importLoaders: 3,
                                sourceMap: isEnvProduction
                                    ? shouldUseSourceMap
                                    : isEnvDevelopment,
                            },
                            'sass-loader'
                        ),
                        // Don't consider CSS imports dead code even if the
                        // containing package claims to have no side effects.
                        // Remove this when webpack adds a warning or an error for this.
                        // See https://github.com/webpack/webpack/issues/6571
                        sideEffects: true,
                    },
                    // Adds support for CSS Modules, but using SASS
                    // using the extension .module.scss or .module.sass
                    {
                        test: sassModuleRegex,
                        use: getStyleLoaders(
                            {
                                importLoaders: 3,
                                sourceMap: isEnvProduction
                                    ? shouldUseSourceMap
                                    : isEnvDevelopment,
                                modules: {
                                    getLocalIdent: getCSSModuleLocalIdent,
                                },
                            },
                            'sass-loader'
                        ),
                    },
                    SVGRLoader(),
                    FileLoader()
                    // ** STOP ** Are you adding a new loader?
                    // Make sure to add the new loader(s) before the "file" loader.
                ],
            },
        ],
    }
}



module.exports = {
    CreateReactAppRules
}


