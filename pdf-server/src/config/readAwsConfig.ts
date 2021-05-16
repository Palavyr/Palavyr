import { isDevelopmentStage } from './currentEnvironment';
import dotenv from 'dotenv';
import { isNullOrUndefined } from '../utils/utils';
import { logDebug } from 'utils/logging';

export const setConfig = () => {
	let currentEnv: dotenv.DotenvConfigOutput;

	if (isDevelopmentStage()) {
		logDebug("IS DEVELOPMNET")
		currentEnv = dotenv.config({ path: '.env.development' });
	} else {
		currentEnv = dotenv.config({ path: '.env.production' });
	}

	if (currentEnv.error) {
		throw new Error('Failed to parse the dotenv file.');
	}
	logDebug("Successfully parsed dotenv file")
};

export const getAwsCredentials = () => {
	if (process.env.AWS_REGION === undefined) setConfig();
	const aws_access_key = process.env.AWS_SECRET_KEY as string;
	const aws_secret_key = process.env.AWS_ACCESS_KEY as string;
	const aws_region = process.env.AWS_REGION as string;

	if (isNullOrUndefined(aws_access_key) || isNullOrUndefined(aws_secret_key) || isNullOrUndefined(aws_region))
		throw new Error('Must call dotenv.config() before access process.env');

	return {
		credentials: {
			secretAccessKey: aws_access_key,
			accessKeyId: aws_secret_key
		},
		region: aws_region
	};
};
