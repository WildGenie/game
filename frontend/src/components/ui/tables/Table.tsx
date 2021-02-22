import * as React from "react"
import { PropsWithChildren } from "react"

import "./Table.scss"

const Table: React.FunctionComponent = ({ children }: PropsWithChildren<unknown>): JSX.Element => {
	return (
		<table className="table">
			{ children }
		</table>
	)
}

export default Table