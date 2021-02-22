import * as React from "react"
import { PropsWithChildren } from "react"

const TableHead: React.FunctionComponent = ({ children }: PropsWithChildren<unknown>): JSX.Element => {
	return (
		<thead className="table__head">
			{children}
		</thead>
	)
}

export default TableHead