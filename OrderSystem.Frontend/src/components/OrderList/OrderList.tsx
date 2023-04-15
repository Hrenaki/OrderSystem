import Order from "../../api/models/common/Order";

export interface OrderListProps {
    orders: Order[],
    onOrderSelect: (orderId: number) => void
}

function OrderList(props: OrderListProps) {
    const listItems = props.orders.map((order, index) => (
            <tr key={order.id} className={order.draft ? 'table-warning' : ''} onClick={() => props.onOrderSelect(order.id)}>
                <th scope="row">{index + 1}</th>
                <td>{order.number}</td>
                <td>{order.providerName}</td>
                <td>{new Date(order.date.toString()).toLocaleDateString('sv')}</td>
            </tr>
        )
    );

    return (
        <table className="table table-hover">
            <thead>
                <tr>
                    <th scope="col">#</th>
                    <th scope="col">Number</th>
                    <th scope="col">Provider</th>
                    <th scope="col">Date</th>
                    </tr>
            </thead>
            <tbody>
                {listItems}
            </tbody>
        </table>
    );
}

export default OrderList;