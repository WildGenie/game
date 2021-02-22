import * as React from "react"
import { PropsWithChildren } from "react"

const TableRow: React.FunctionComponent = ({ children }: PropsWithChildren<unknown>): JSX.Element => {
	return (
		<tr className="table__row">
			{ children }
		</tr>
	)
}

export default TableRow