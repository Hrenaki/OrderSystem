import 'bootstrap/dist/css/bootstrap.css';
import { useState } from 'react';

interface MultiSelectListItemProps {
    option: string,
    onSelect: () => void
};

function MultiSelectListItem(props: MultiSelectListItemProps) {
    return (
        <div className="list-group-item" onChange={props.onSelect}>
            <input className="form-check-input me-2" type="checkbox"/>
            {props.option}
        </div>
    );
};

export interface MultiSelectListProps {
    options: any[]
};

function MultiSelectList(props: MultiSelectListProps) {
    const [selected, setSelected] = useState(Array<any>(0));

    const items = props.options.map(o =>
        <MultiSelectListItem key={o} option={o}
            onSelect={() => {
                console.log(selected);
                if(selected.some(s => s === o))
                    setSelected(selected.filter(s => s !== o));
                else setSelected([...selected, o]);
            }}
        />
    );

    return (
        <ul className="list-group text-start">
            {items}
        </ul>
    );
};

export default MultiSelectList;