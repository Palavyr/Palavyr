const TypeScriptLoaderRule = () => {
    return {
        test: /\.ts?$/,
        use: [
            { loader: 'ts-loader', options: { transpileOnly: true } },
        ],
        exclude: /node_modules/
    }
}


const BabelLoaderRule = () => {
    return {
        test: /\.js$/,
        use: [
            { loader: 'babel-loader' }
        ],
        exclude: /node_modules/
    }
}


module.exports = {
    TypeScriptLoaderRule,
    BabelLoaderRule
}