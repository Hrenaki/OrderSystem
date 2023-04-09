import 'bootstrap/dist/css/bootstrap.css';
import { useState } from 'react';

interface MultiSelectListItemProps {
    option: SelectListOption,
    onSelect: () => void
};

function MultiSelectListItem(props: MultiSelectListItemProps) {
    return (
        <div className="list-group-item" onChange={props.onSelect}>
            <input className="form-check-input me-2" type="checkbox"/>
            <input type="hidden" value={props.option.key}/>
            {props.option.value}
        </div>
    );
};

export interface SelectListOption {
    key: string,
    value: string
}

export interface MultiSelectListProps {
    options: SelectListOption[]
};

function MultiSelectList(props: MultiSelectListProps) {
    const [selected, setSelected] = useState(Array<any>(0));

    const items = props.options.map(o =>
        <MultiSelectListItem key={o.key} option={o}
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