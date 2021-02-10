import React from 'react';
import cn from 'classnames';

import './styles.scss';
import { Box } from '@material-ui/core';

type Props = {
  typing: boolean;
}

function Loader({ typing }: Props) {
  return (
    <div className={cn('loader', { active: typing })}>
      <Box  boxShadow={1} className="loader-container">
        <span className="loader-dots"></span>
        <span className="loader-dots"></span>
        <span className="loader-dots"></span>
      </Box>
    </div>
  );
}

export default Loader;
