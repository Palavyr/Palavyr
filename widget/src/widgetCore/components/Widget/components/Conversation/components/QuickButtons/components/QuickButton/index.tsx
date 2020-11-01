import React from 'react';

import { QuickButton } from 'src/widgetCore/store/types';
import './styles.scss';

type Props = {
  button: QuickButton;
  onQuickButtonClicked: (event: any, value: string | number) => any;
}

function QuickButton({ button, onQuickButtonClicked }: Props) {
  return (
    <button className="quick-button" onClick={(event) => onQuickButtonClicked(event, button.value)}>
      {button.label}
    </button>
  );
}

export default QuickButton;
