import * as React from "react"
import { PropsWithChildren } from "react"

const TableBody: React.FunctionComponent = ({ children }: PropsWithChildren<unknown>): JSX.Element => {
	return (
		<tbody className="table__body">
			{ children }
		</tbody>
	)
}

export default TableBody